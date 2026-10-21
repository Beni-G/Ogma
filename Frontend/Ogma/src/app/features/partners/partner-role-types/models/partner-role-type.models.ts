export interface PartnerRoleType {
  id: number;
  code: string;
  name: string;
  color?: string;
}

export type CreatePartnerRoleTypeDto = Omit<PartnerRoleType, 'id'>;
export type UpdatePartnerRoleTypeDto = PartnerRoleType;