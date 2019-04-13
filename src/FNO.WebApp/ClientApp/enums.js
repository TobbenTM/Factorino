// Mapped backend enums

export const FactoryState = {
  Unknown: 0,
  Creating: 1,
  Starting: 2,
  Online: 3,
  Destroying: 4,
  Destroyed: 5,
};

export const OrderType = {
  Buy: 0,
  Sell: 1,
};

export const OrderState = {
  Created: 0,
  Active: 1,
  Fulfilled: 2,
  Cancelled: 3,
};
