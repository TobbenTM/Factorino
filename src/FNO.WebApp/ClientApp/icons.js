import fontawesome from '@fortawesome/fontawesome';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

fontawesome.library.add(
  // General
  require('@fortawesome/fontawesome-free-solid/faHome'),
  require('@fortawesome/fontawesome-free-solid/faGlobe'),
  require('@fortawesome/fontawesome-free-solid/faSpinner'),
  require('@fortawesome/fontawesome-free-solid/faIndustry'),
  require('@fortawesome/fontawesome-free-solid/faWarehouse'),
  require('@fortawesome/fontawesome-free-solid/faBuilding'),
  require('@fortawesome/fontawesome-free-solid/faCoins'),
  require('@fortawesome/fontawesome-free-solid/faServer'),
  require('@fortawesome/fontawesome-free-solid/faLock'),

  // Brands
  require('@fortawesome/fontawesome-free-brands/faSteam'),
);

export default FontAwesomeIcon;
