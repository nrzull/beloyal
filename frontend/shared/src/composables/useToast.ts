import { useMessage } from "naive-ui";

const GENERIC_SUCCESS = "Успешно";
const GENERIC_ERROR = "Ошибка";

export const useToast = () => {
  const { success, error } = useMessage();

  return {
    showOk: (msg = GENERIC_SUCCESS) => success(msg),
    showError: (msg = GENERIC_ERROR) => error(msg),
  };
};
