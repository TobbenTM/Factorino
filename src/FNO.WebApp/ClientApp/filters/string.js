
export function capitalize(value) {
  return value.charAt(0).toUpperCase() + value.slice(1);
}

export function separate(value, separator, replacement) {
  return value.replace(separator || /-/g, replacement || ' ');
}
