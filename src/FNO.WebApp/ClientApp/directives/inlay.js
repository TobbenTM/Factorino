import variables from '@/css/variables.scss';

export default {
  bind(el, { arg }) {
    Object.assign(el.style, {
      position: 'relative',
      borderRadius: '6px',
      overflow: 'hidden',
      borderTop: `2px solid ${variables.embossDarkColor}`,
      borderBottom: `2px solid ${variables.embossLightColor}`,
    });

    switch (arg) {
      case 'dark':
        el.style.background = variables.inlayDarkColor;
        break;
      case 'light':
        el.style.background = variables.inlayLightColor;
        break;
      default:
        break;
    }

    const mask = document.createElement('div');
    Object.assign(mask.style, {
      position: 'absolute',
      width: '100%',
      height: '100%',
      top: '0',
      left: '0',
      pointerEvents: 'none',
      boxShadow: 'inset 0 0 10px rgba(0, 0, 0, 1)',
    });
    el.appendChild(mask);
  },
};
