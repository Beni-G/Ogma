export interface Category {
  id: number;
  name: string;
  parentCategoryId?: number | null;
  ancestors?: Category[] | null;
  subCategories?: Category[] | null;
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