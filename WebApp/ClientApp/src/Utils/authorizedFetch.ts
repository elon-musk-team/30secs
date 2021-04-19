import authService from "../components/api-authorization/AuthorizeService";

/**
 * подставляет беарер сразу в хедеры и преобразовывает ответ из json в объект js
 **/
export const authorizedFetch: any = async (input: RequestInfo, init: RequestInit = {}) => {
  const token = await authService.getAccessToken();
  if (token) {
    init.headers = {'Authorization': `Bearer ${token}`, ...init.headers};
  }
  const response = await fetch(input, init);
  return await response.json();
};
