import styles from "./backBtn.module.scss";
import arrow from "../../../assets/images/arrow.svg";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/constants";

export const BackBtn = () => {
  const navigate = useNavigate();

  return (
    <button
      className={styles.backBtn}
      type="button"
      onClick={() => {
        navigate(ROUTES.RECIPES);
      }}
    >
      <img src={arrow} alt="back" />
      Назад
    </button>
  );
};
