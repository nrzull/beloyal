import { ref } from "vue";

type Fn = (...args: any[]) => any;

const storage = ref(new Set<string | Fn>([]));
const some = (v: (string | Fn)[]) => v.some((vv) => storage.value.has(vv));
const start = (v: string | Fn) => storage.value.add(v);
const end = (v: string | Fn) => storage.value.delete(v);

export const wait = { start, end, some };

export const waitable = <T extends (...args: any) => any>(v: T) =>
  (async (...args: any) => {
    try {
      wait.start(v);
      return await v(...args);
    } finally {
      wait.end(v);
    }
  }) as T;
