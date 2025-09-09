import React from 'react';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ children }) => {
  const token = localStorage.getItem('token');

  if (!token) {
    // Если токена нет, перенаправляем на страницу входа
    return <Navigate to="/login" replace />;
  }

  // Если токен есть, показываем дочерние компоненты (приватная страница)
  return children;
};

export default PrivateRoute;
