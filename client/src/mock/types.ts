export type MockApi = {
  url: string,
  method: string,
  response: () => Record<string, any>,
};
