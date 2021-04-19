import authService from "../components/api-authorization/AuthorizeService";

/**
 * подставляет беарер сразу в хедеры
 **/
export const authorizedFetch = async (input: RequestInfo, init: RequestInit = {}) => {
  const token = await authService.getAccessToken();
  if (token) {
    init.headers = {'Authorization': `Bearer ${token}`, ...init.headers};
  }
  return await fetch(input, init);
};
