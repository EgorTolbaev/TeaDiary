import React, { useEffect, useState, useContext } from 'react';
import { Navigate } from 'react-router-dom';
import apiClient from '../api';
import { AuthContext } from '../context/AuthContext';

const Profile = () => {
  const { token, user, login, logout } = useContext(AuthContext);
  const [loading, setLoading] = useState(!user);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!token) {
      setLoading(false);
      return;
    }
    if (!user) {
      const fetchProfile = async () => {
        try {
          const response = await apiClient.get('/Auth/me', {
            headers: { Authorization: `Bearer ${token}` }
          });
          login(token, response.data); // Сохраняем профиль в контексте
        } catch (err) {
          setError(err.response?.data?.message || 'Ошибка загрузки профиля');
          logout(); // Токен невалиден или ошибка - выходим
        } finally {
          setLoading(false);
        }
      };
      fetchProfile();
    } else {
      setLoading(false);
    }
  }, [token, user, login, logout]);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (loading) return <div>Загрузка профиля...</div>;
  if (error) return <div style={{ color: 'red' }}>{error}</div>;
  if (!user) return <div>Нет данных.</div>;

  return (
    <div>
      <h2>Профиль пользователя</h2>
      <p><strong>Имя:</strong> {user.firstName}</p>
      <p><strong>Фамилия:</strong> {user.lastName}</p>
      <p><strong>Отчество:</strong> {user.middleName}</p>
      <p><strong>Email:</strong> {user.email}</p>
      <p><strong>Дата регистрации:</strong> {user.createdAt ? new Date(user.createdAt).toLocaleString() : '-'}</p>
    </div>
  );
};

export default Profile;
