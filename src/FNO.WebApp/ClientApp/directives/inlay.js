import variables from '@/css/variables.scss';

export default {
  bind(el, { arg, modifiers }) {
    Object.assign(el.style, {
      position: 'relative',
      borderRadius: modifiers.square ? '2px' : '6px',
      overflowX: 'hidden',
      overflowY: 'hidden',
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
    el.dataset.maskId = Math.random().toString(36).substr(2, 9);
    mask.id = el.dataset.maskId;
    el.appendChild(mask);
  },
  unbind(el) {
    const mask = document.getElementById(el.dataset.maskId);
    if (mask) {
      mask.remove();
    }
  },
};
