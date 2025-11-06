import {
  adminRolePermissions,
  type AdminRolePermission,
} from "@beloyal/shared";
import { ref, computed, type ComputedRef } from "vue";
import { globalI18n } from "~/locales";
import { apiSelfGet } from "~/api/self";
import { getKnownLocale } from "~/locales/utils";

export const self = ref<Awaited<ReturnType<typeof apiSelfGet>>>();

export const fetchSelf = async () => {
  try {
    self.value = await apiSelfGet();
    globalI18n.global.locale.value = getKnownLocale(
      self.value.user.countryLang
    );
  } catch (e) {
    console.error(e);
  }
};

export const hasAdminPermission = (v: AdminRolePermission) =>
  Boolean(
    isAdmin.value && self.value?.adminAccess?.role?.permissions?.includes(v)
  );

export const isAdmin = computed(
  () => self.value && self.value?.adminAccess && !self.value.companyAccess
);

export const { admin_white_companies_read, admin_white_companies_write } =
  Array.from(adminRolePermissions.values()).reduce<
    Record<`admin_${AdminRolePermission}`, ComputedRef<boolean>>
  >((acc, v) => {
    acc[`admin_${v}`] = computed(() => hasAdminPermission(v));
    return acc;
  }, {} as any);
