import styles from "./stepPart.module.scss";
import { Step } from "../../../../types/recipe";

type StepPartProps = {
  steps: Step[];
};

export const StepPart = (props: StepPartProps) => {
  return (
    <ul className={styles.stepsBlock}>
      {props.steps.length > 0 ? (
        props.steps.map((step, index) => (
          <li key={step.id} className={styles.stepInfo}>
            <p className={styles.stepTitle}>Шаг {index + 1}</p>
            <p className={styles.stepDescription}>{step.description}</p>
          </li>
        ))
      ) : (
        <p className={styles.notFoundText}>Шаги не найдены.</p>
      )}
      <p className={styles.bonAppetitText}>Приятного аппетита!</p>
    </ul>
  );
};
