import styles from "./userProfileHeader.module.scss";
import { BackBtn } from "../../../customComponents/backBtn/backBtn";

export const UserProfileHeader = () => {
  return (
    <div className={styles.container}>
      <BackBtn />
      <h2 className={styles.title}>Мой профиль</h2>
    </div>
  );
};
