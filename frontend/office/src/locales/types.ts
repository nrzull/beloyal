import type { knownLocales } from "~/locales/utils";
export type CountryLang = keyof typeof knownLocales;

export type CountryMessages = {
  shops: {
    menuTitle: string;
  };

  analytics: {
    menuTitle: string;
    mainMenuTitle: string;
    operationsMenuTitle: string;
  };

  marketing: {
    menuTitle: string;
    bonusesMenuTitle: string;
    promotionsMenuTitle: string;
    levelsMenuTitle: string;
    mailingsMenuTitle: string;
    qrStickersMenuTitle: string;
  };

  clients: {
    menuTitle: string;
  };

  chats: {
    menuTitle: string;
  };

  staff: {
    menuTitle: string;
    companyAccessesMenuTitle: string;
    rolesMenuTitle: string;
  };

  settings: {
    menuTitle: string;
    companyMenuTitle: string;
    telegramMenuTitle: string;
  };
};
