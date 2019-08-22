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
  PartiallyFulfilled: 2,
  Fulfilled: 3,
  Cancelled: 4,
  // Some front-end specific ones:
  Cancelling: 101,
};

export const OrderCancellationReason = {
  Unknown: 0,
  User: 1,
  NoFunds: 2,
  NoResources: 3,
};

export const OrderSearchSortColumn = {
  Default: 0,
  Item: 1,
  Player: 2,
  Price: 3,
};

export const ShipmentState = {
  Unknown: 0,
  Requested: 1,
  Fulfilled: 2,
  Received: 3,
  Completed: 4,
};

export const CartType = {
  Unknown: 0,
  Cargo: 1,
  Fluid: 2,
};
