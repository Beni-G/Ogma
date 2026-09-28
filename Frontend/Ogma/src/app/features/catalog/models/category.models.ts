export interface Category {
  id: number;
  name: string;
  parentCategoryId?: number | null;
  children?: Category[];
}

export interface CreateCategoryCommand {
  name: string;
  parentCategoryId?: number | null;
}

export interface UpdateCategoryCommand {
  id: number;
  name: string;
  parentCategoryId?: number | null;
}