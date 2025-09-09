import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api';
import { AuthContext } from '../context/AuthContext';

const Login = () => {
  const [formData, setFormData] = useState({ email: '', password: '' });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await apiClient.post('/Auth/login', formData);
      const token = response.data.token;

      // Получаем профиль пользователя
      const userResponse = await apiClient.get('/Auth/me', {
        headers: { Authorization: `Bearer ${token}` }
      });

      // Обновляем состояние аутентификации через контекст
      login(token, userResponse.data);

      setLoading(false);
      navigate('/profile'); // Перенаправление на страницу профиля
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка входа');
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Вход</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <form onSubmit={handleSubmit}>
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleChange}
          required
        />
        <br />
        <input
          type="password"
          name="password"
          placeholder="Пароль"
          value={formData.password}
          onChange={handleChange}
          required
          minLength={6}
        />
        <br />
        <button type="submit" disabled={loading}>
          {loading ? 'Вхожу...' : 'Войти'}
        </button>
      </form>
    </div>
  );
};

export default Login;
