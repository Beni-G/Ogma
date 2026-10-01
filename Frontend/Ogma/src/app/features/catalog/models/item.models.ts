import { Money } from "../../shared-kernel/models/money.model";
import { Category } from "./category.models";
import { ItemType } from "./item-type.models";

export interface Item {
  id: number;
  name: string;
  code: string;
  description: string;
  categoryId: number;
  category: Category;
  listPrice: Money;
  itemTypeId: number;
  itemType: ItemType;
  unitOfMeasurement: string;
  isActive: boolean;
}

export interface CreateItemCommand {
  name: string;
  code: string;
  description: string;
  categoryId: number;
  listPrice: Money;
  itemTypeId: number;
  unitOfMeasurement: string;
  isActive: boolean;
}

export type UpdateItemCommand = CreateItemCommand & { id: number };