import MyButton from "../../customComponents/myButton/myButton";
import styles from "./loginOrRegistrationPopup.module.scss";
import closeIcon from "../../../assets/images/close.svg";
import { usePopupStore } from "../../../hooks/usePopupStore";

export const LoginOrRegistrationPopup = () => {
  const { setIsLoginOrRegistraionPopupOpen, setIsLoginPopupOpen, setIsRegistrationPopupOpen } = usePopupStore();

  const handleClosePopup = () => {
    setIsLoginOrRegistraionPopupOpen(false);
  };

  const navigateToLogin = () => {
    setIsLoginOrRegistraionPopupOpen(false);
    setIsLoginPopupOpen(true);
  };

  const navigateToRegistration = () => {
    setIsLoginOrRegistraionPopupOpen(false);
    setIsRegistrationPopupOpen(true);
  };

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <button onClick={handleClosePopup} className={styles.closeBtn}>
          <img src={closeIcon} alt="close" className={styles.closeIcon} />
        </button>
        <p className={styles.title}>Войдите в профиль</p>
        <p className={styles.description}>Добавлять рецепты могут только зарегистрированные пользователи.</p>
        <div className={styles.buttons}>
          <MyButton isPrimary={true} onClick={navigateToLogin} width="278px" height="60px">
            Войти
          </MyButton>
          <MyButton isPrimary={false} onClick={navigateToRegistration} width="278px" height="60px">
            Регистрация
          </MyButton>
        </div>
      </div>
    </div>
  );
};
