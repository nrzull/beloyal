import { onUnmounted, onMounted } from "vue";

export const useMobileVh = (
  params: { variableName: string } = { variableName: "--vh1" }
) => {
  const onResize = () => {
    const vh = window.innerHeight * 0.01;
    document.documentElement.style.setProperty(params.variableName, `${vh}px`);
  };

  window.addEventListener("resize", onResize);

  onMounted(onResize);
  onUnmounted(() => window.removeEventListener("resize", onResize));
};
