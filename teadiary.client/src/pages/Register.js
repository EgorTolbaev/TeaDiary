import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api';
import { AuthContext } from '../context/AuthContext';

const Register = () => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    middleName: '',
    email: '',
    password: '',
    confirmPassword: '',
    avatarId: ''
  });

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

    if (formData.password !== formData.confirmPassword) {
      setError('Пароли не совпадают');
      return;
    }

    setLoading(true);

    try {
      await apiClient.post('/Auth/register', formData);

      // После успешной регистрации сразу выполнить логин
      const loginResponse = await apiClient.post('/Auth/login', {
        email: formData.email,
        password: formData.password,
      });

      const token = loginResponse.data.token;

      // Получить данные пользователя
      const userResponse = await apiClient.get('/Auth/me', {
        headers: { Authorization: `Bearer ${token}` }
      });

      login(token, userResponse.data); // обновляем контекст

      setLoading(false);
      navigate('/profile'); // переходим на профиль
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка регистрации');
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Регистрация</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="firstName"
          placeholder="Имя"
          value={formData.firstName}
          onChange={handleChange}
          required
          minLength={2}
          maxLength={50}
        />
        <br />
        <input
          type="text"
          name="lastName"
          placeholder="Фамилия"
          value={formData.lastName}
          onChange={handleChange}
          maxLength={50}
        />
        <br />
        <input
          type="text"
          name="middleName"
          placeholder="Отчество"
          value={formData.middleName}
          onChange={handleChange}
          maxLength={50}
        />
        <br />
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
          maxLength={100}
        />
        <br />
        <input
          type="password"
          name="confirmPassword"
          placeholder="Подтверждение пароля"
          value={formData.confirmPassword}
          onChange={handleChange}
          required
          minLength={6}
          maxLength={100}
        />
        <br />
        {/* Опционально поле аватара */}
        {/* <input
          type="text"
          name="avatarId"
          placeholder="ID аватара (опционально)"
          value={formData.avatarId}
          onChange={handleChange}
        />
        <br /> */}
        <button type="submit" disabled={loading}>
          {loading ? 'Регистрируем...' : 'Зарегистрироваться'}
        </button>
      </form>
    </div>
  );
};

export default Register;
