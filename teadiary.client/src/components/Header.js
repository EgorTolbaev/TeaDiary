import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const Header = () => {
  const navigate = useNavigate();
  const { token, logout } = useContext(AuthContext);

  const handleLogout = () => {
    logout();           // вызываем метод из контекста, который удалит токен и очистит состояние
    navigate('/login'); // переходим на страницу входа
  };

  return (
    <header style={{ padding: '10px', borderBottom: '1px solid #ccc' }}>
      <nav>
        <Link to="/" style={{ marginRight: 15 }}>Главная</Link>

        {!token && (
          <>
            <Link to="/login" style={{ marginRight: 15 }}>Вход</Link>
            <Link to="/register" style={{ marginRight: 15 }}>Регистрация</Link>
          </>
        )}

        {token && (
          <>
            <Link to="/profile" style={{ marginRight: 15 }}>Профиль</Link>
            <button onClick={handleLogout}>Выйти</button>
          </>
        )}
      </nav>
    </header>
  );
};

export default Header;
