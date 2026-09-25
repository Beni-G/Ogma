export interface ItemType{
    id: number;
    name: string;
    description: string;
}

export interface CreateItemTypeCommand {
  name: string;
  description: string;
}

export interface UpdateItemTypeCommand {
  id: number;
  name: string;
  description: string;
}