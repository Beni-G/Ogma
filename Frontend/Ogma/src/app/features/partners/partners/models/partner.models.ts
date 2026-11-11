import { PartnerRoleType } from "../../partner-role-types/models/partner-role-type.models";

export interface Address {
  street?: string;
  number?: string;
  city?: string;
  region?: string;
  postalCode?: string;
  countryCode?: string;
  building?: string;
  stairCase?: string;
  floor?: string;
  apartment?: string;
}

export interface ValidityPeriod {
  start?: string | null;
  end?: string | null;
}

export interface PartnerIdentifier {
  id?: number;
  type: string;
  value: string;
  validityPeriod?: ValidityPeriod;
  isPrimary: boolean;
}

export interface BankAccountDetails {
  bank?: string;
  iban: string;
  currency?: string;
  bic?: string;
}

export interface PartnerBankAccount {
  id?: number;
  bankAccount: BankAccountDetails;
  isDefault: boolean;
}

export interface PersonName {
  firstName: string;
  lastName: string;
}

export interface PartnerContact {
  id?: number;
  name: PersonName;
  email?: string;
  phone?: string;
  mobile?: string;
  title?: string;
  jobTitle?: string;
  isPrimary: boolean;
}

export interface Partner {
  id: number;
  individualName?: PersonName;
  companyName?: string;
  isNaturalPerson: boolean;
  isActive?: boolean;
  displayName?: string;
  hqAddress?: Address;
  identifiers?: PartnerIdentifier[];
  roles?: PartnerRoleType[];
  bankAccounts?: PartnerBankAccount[];
  contacts?: PartnerContact[];
}

export interface CreatePartnerRequest {
  individualName?: PersonName;
  companyName?: string;
  isNaturalPerson: boolean;
  hqAddress?: Address;
  identifier: PartnerIdentifier;
  roleId: number;
}

export interface UpdatePartnerRequest {
  id: number;
  individualName?: PersonName;
  companyName?: string;
  isNaturalPerson: boolean;
  isActive: boolean;
  displayName?: string;
  hqAddress: Address;
  identifiers: PartnerIdentifier[];
  roleIds: number[];
  bankAccounts: PartnerBankAccount[];
  contacts: PartnerContact[];
}