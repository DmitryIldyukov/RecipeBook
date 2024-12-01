import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/images/close.svg";
import styles from "./registrationPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { useState } from "react";
import { LoginInfo, RegistrationInfo } from "../../../types/auth";
import { useAppStore } from "../../../hooks/useStore";
import { authService } from "../../../services/authService";
import { handleError } from "../../../utils/errorHandler";
import toast from "react-hot-toast";

export const RegistrationPopup = () => {
  const { setIsRegistrationPopupOpen, setIsLoginPopupOpen } = usePopupStore();
  const { login } = useAppStore();

  const [name, setName] = useState("");
  const [loginData, setLoginData] = useState("");
  const [password, setPassword] = useState("");
  const [chechPassword, setChechPassword] = useState("");
  const [errors, setErrors] = useState<{ name?: string; login?: string; password?: string }>({});

  const handleClosePopup = () => {
    setIsRegistrationPopupOpen(false);
  };

  const navigateToLogin = () => {
    setIsRegistrationPopupOpen(false);
    setIsLoginPopupOpen(true);
  };

  const validate = () => {
    const newErrors: { name?: string; login?: string; password?: string } = {};

    if (!name) {
      newErrors.name = "Имя обязательно";
    }

    if (!loginData) {
      newErrors.login = "Логин обязателен";
    }

    if (password.length < 8) {
      newErrors.password = "Пароль должен содержать минимум 8 символов";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleRegistration = async () => {
    if (!validate()) {
      return;
    }

    const data: RegistrationInfo = { name: name, login: loginData, password: password };

    if (password !== chechPassword) {
      toast.error("Пароли не совпадают");
      return;
    }

    await authService
      .Registration(data)
      .then(handleLogin)
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при регистрации");
      });
  };

  const handleLogin = () => {
    const data: LoginInfo = { login: loginData, password: password };

    login(data)
      .then(() => {
        handleClosePopup();
      })
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при авторизации");
      });
  };

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <button onClick={handleClosePopup} className={styles.closeBtn}>
          <img src={closeIcon} alt="close" className={styles.closeIcon} />
        </button>
        <p className={styles.title}>Регистрация</p>
        <form className={styles.form}>
          <div className={styles.formInputs}>
            <div className={styles.inputBlock}>
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
              {errors.name && <div className={styles.errorText}>{errors.name}</div>}
            </div>

            <div className={styles.inputBlock}>
              <input
                type="text"
                placeholder="Логин"
                className={styles.input}
                id="login"
                value={loginData}
                onChange={(e) => {
                  setLoginData(e.target.value);
                }}
              />
              {errors.login && <div className={styles.errorText}>{errors.login}</div>}
            </div>

            <div className={styles.inputBlock}>
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
              {errors.password && <div className={styles.errorText}>{errors.password}</div>}
            </div>
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
