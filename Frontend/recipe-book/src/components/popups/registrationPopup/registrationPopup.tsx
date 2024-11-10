import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/close.svg";
import styles from "./registrationPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import AuthService from "../../../services/authService";
import { useState } from "react";
import { RegistrationInfo } from "../../../types/auth";

const authService = new AuthService();

export const RegistrationPopup = () => {
  const { setIsRegistrationPopupOpen, setIsLoginPopupOpen } = usePopupStore();

  const [name, setName] = useState("");
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [chechPassword, setChechPassword] = useState("");

  const handleClosePopup = () => {
    setIsRegistrationPopupOpen(false);
  };

  const navigateToLogin = () => {
    setIsRegistrationPopupOpen(false);
    setIsLoginPopupOpen(true);
  };

  const handleRegistration = async () => {
    const data: RegistrationInfo = { name: name, login: login, password: password };

    if (password !== chechPassword) {
      return;
    }

    try {
      const response = await authService.Registration(data);
      if (response.ok) {
        handleClosePopup();
      } else {
        const errorMessages = await response.json();
        console.error(errorMessages);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <button onClick={handleClosePopup} className={styles.closeBtn}>
          <img src={closeIcon} alt="close" className={styles.closeIcon} />
        </button>
        <p className={styles.title}>Регистрация</p>
        <form className={styles.form}>
          <input
            type="text"
            placeholder="Имя"
            className={styles.input}
            id="name"
            value={name}
            onChange={(e) => {
              setName(e.target.value);
            }}
          />
          <input
            type="text"
            placeholder="Логин"
            className={styles.input}
            id="login"
            value={login}
            onChange={(e) => {
              setLogin(e.target.value);
            }}
          />

          <div className={styles.passwordContainer}>
            <div className={styles.password}>
              <input
                type="password"
                placeholder="Пароль"
                className={styles.passInput}
                id="password"
                value={password}
                onChange={(e) => {
                  setPassword(e.target.value);
                }}
              />
              <span className={styles.description}>Минимум 8 символов</span>
            </div>
            <input
              type="password"
              placeholder="Пароль"
              className={styles.input}
              id="password"
              value={chechPassword}
              onChange={(e) => {
                setChechPassword(e.target.value);
              }}
            />
          </div>

          <div className={styles.buttons}>
            <MyButton isPrimary={true} onClick={() => void handleRegistration()} width="278px" height="60px">
              Зарегистрироваться
            </MyButton>
            <MyButton isPrimary={false} onClick={handleClosePopup} width="278px" height="60px">
              Отмена
            </MyButton>
          </div>
        </form>
        <div className={styles.registration}>
          <button onClick={navigateToLogin} className={styles.registrationLink}>
            У меня уже есть аккаунт
          </button>
        </div>
      </div>
    </div>
  );
};
