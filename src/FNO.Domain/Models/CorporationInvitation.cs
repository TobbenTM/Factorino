using System;
using System.ComponentModel.DataAnnotations;

namespace FNO.Domain.Models
{
    public class CorporationInvitation
    {
        [Key]
        public Guid InvitationId { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }

        public bool Accepted { get; set; }
        public bool Completed { get; set; }
    }
}
