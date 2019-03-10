import { formatDistance } from 'date-fns/esm';

export function formatTimestamp(timestamp) {
  if (!timestamp) return '';
  return formatDistance(new Date(timestamp * 1000), new Date(), { addSuffix: true });
}

export default {
  formatTimestamp,
};
