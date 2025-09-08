import { ref } from "vue";

type Fn = (...args: any[]) => any;

const storage = ref(new Set<string | Fn>([]));
const some = (v: (string | Fn)[]) => v.some((vv) => storage.value.has(vv));
const start = (v: string | Fn) => storage.value.add(v);
const end = (v: string | Fn) => storage.value.delete(v);

export const wait = { start, end, some };
