import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5069/api', // базовый URL API
  headers: {
    'Content-Type': 'application/json'
  }
});

// Добавляем интерцептор для добавления JWT токена в заголовки запросов
apiClient.interceptors.request.use(config => {
  const token = localStorage.getItem('token'); // или где храните токен
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;
