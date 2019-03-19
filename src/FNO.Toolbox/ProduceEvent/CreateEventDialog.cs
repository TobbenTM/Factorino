﻿using FNO.Domain.Events;
using FNO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Terminal.Gui;

namespace FNO.Toolbox.ProduceEvent
{
    internal class CreateEventDialog : Dialog
    {
        public CreateEventDialog(Type eventType) : base("Create event", 0, 0)
        {
            var supportedPrimitives = new[]
            {
                typeof(int),
                typeof(long),
                typeof(double),
                typeof(float),
                typeof(string),
                typeof(Guid),
            };
            var props = eventType.GetProperties()
                .Where(p => p.CanWrite)
                .Where(p => p.PropertyType != typeof(EventInitiator))
                .Where(p => p.PropertyType != typeof(EventMetadata))
                .ToList();

            Width = 52;
            Height = props.Count * 2 + 8;

            Add(new Label(eventType.Name)
            {
                X = Pos.Center(),
                Y = 1,
            });

            var formLabels = new List<(View view, PropertyInfo prop)>();
            var formFields = new List<TextField>();

            foreach (var prop in props)
            {
                var label = new Label(prop.Name)
                {
                    X = 2,
                    Y = formLabels.Count * 2 + 3, // formLabels.Any() ? Pos.Top(formLabels.Last().view) + 2 : 3,
                    Width = 20,
                    Height = 1,
                };
                formLabels.Add((label, prop));
                Add(label);

                //if (!supportedPrimitives.Contains(prop.PropertyType))
                //{
                //    var innerProps = prop.PropertyType.GetProperties()
                //        .Where(p => p.CanWrite)
                //        .ToList();

                //    foreach (var inner in innerProps)
                //    {
                //        var innerLabel = new Label(inner.Name)
                //        {
                //            X = 4,
                //            Y = Pos.Top(formLabels.Last().view) + 2,
                //            Width = 18,
                //        };
                //        formLabels.Add((innerLabel, inner));
                //        Add(innerLabel);
                //    }
                //}
            }

            foreach (var (label, prop) in formLabels)
            {
                if (supportedPrimitives.Contains(prop.PropertyType))
                {
                    var field = new TextField("")
                    {
                        Id = prop.Name,
                        X = 22,
                        Y = Pos.Top(label),
                        Width = 22,
                        Height = 1,
                    };
                    formFields.Add(field);
                    Add(field);
                }
                else if (prop.PropertyType == typeof(LuaItemStack[]))
                {
                    Add(new Label("<auto-generated>")
                    {
                        X = 22,
                        Y = Pos.Top(label),
                        Width = 22,
                        Height = 1,
                    });
                }
            }

            Add(new Button("Create")
            {
                X = Pos.Center(),
                Y = formLabels.Count * 2 + 3,
                Clicked = delegate ()
                {
                    var evnt = Activator.CreateInstance(eventType);

                    // Parse form fields and set values
                    foreach (var field in formFields.Where(f => !string.IsNullOrEmpty(f.Text.ToString())))
                    {
                        var prop = eventType.GetProperty(field.Id.ToString());
                        if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(evnt, field.Text.ToString());
                        }
                        else if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(evnt, int.Parse(field.Text.ToString()));
                        }
                        else if (prop.PropertyType == typeof(long))
                        {
                            prop.SetValue(evnt, long.Parse(field.Text.ToString()));
                        }
                        else if (prop.PropertyType == typeof(double))
                        {
                            prop.SetValue(evnt, double.Parse(field.Text.ToString()));
                        }
                        else if (prop.PropertyType == typeof(float))
                        {
                            prop.SetValue(evnt, float.Parse(field.Text.ToString()));
                        }
                        else if (prop.PropertyType == typeof(Guid))
                        {
                            prop.SetValue(evnt, Guid.Parse(field.Text.ToString()));
                        }
                    }

                    // Set auto-generated values for certain prop types
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(LuaItemStack[]))
                        {
                            var stack = new[]
                            {
                                new LuaItemStack
                                {
                                    Name = "iron-plate",
                                    Count = 420,
                                },
                            };
                            prop.SetValue(evnt, stack);
                        }
                    }

                    if (evnt is Event baseEvent)
                    {
                        baseEvent.Initiator = new EventInitiator
                        {
                            PlayerName = "<toolbox>",
                        };
                        baseEvent.Metadata = new EventMetadata
                        {
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        };
                        var producer = new ProducerDialog();
                        producer.Produce(baseEvent);
                        Application.Run(producer);
                    }
                    else
                    {
                        throw new Exception("Could not instantiate the event correctly!");
                    }
                }
            });
        }
    }
}
