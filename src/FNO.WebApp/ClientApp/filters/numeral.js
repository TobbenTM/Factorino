import numeral from 'numeral';

export default function (number, format) {
  return numeral(number).format(format || '0,0.0000');
}
