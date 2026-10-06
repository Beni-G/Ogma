import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ConfirmationService, TreeNode } from 'primeng/api';
import { TreeTableModule } from 'primeng/treetable';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Category, CreateCategoryCommand, UpdateCategoryCommand } from '../../models/category.models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-categories-list',
  imports: [
    CommonModule,
    FormsModule,
    ConfirmDialogModule,
    TreeTableModule,
    ButtonModule,
    DialogModule,
    SelectModule,
    InputTextModule,
    TooltipModule
  ],
  providers: [ConfirmationService],
  templateUrl: './categories-list.component.html',
  styleUrl: './categories-list.component.scss'
})

export class CategoriesListComponent implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly confirmationService = inject(ConfirmationService);

  flatCategories = signal<Category[]>([]);
  treeNodes = signal<TreeNode[]>([]);

  loading = signal(false);
  isDialogOpen = signal(false);
  dialogHeader = signal('Add Category');

  selectedCategoryId = signal<number | null>(null);
  selectedParentId = signal<number | null>(null);
  categoryName = signal('');

  availableParents = computed(() => {
    const currentId = this.selectedCategoryId();
    const all = this.flatCategories();

    if (!currentId) {
      return all
    };

    const invalidIds = new Set<number>();
    const collectDescendants = (id: number) => {
      invalidIds.add(id);
      all.filter(c => c.parentCategoryId === id).forEach(child => collectDescendants(child.id));
    };
    collectDescendants(currentId);

    return all.filter(c => !invalidIds.has(c.id));
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading.set(true);
    this.categoryService.getAllFlat().subscribe({
      next: (categories) => {
        this.flatCategories.set(categories);
        this.treeNodes.set(this.buildTreeNodes(categories));
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  private buildTreeNodes(categories: Category[], parentId: number | null = null): TreeNode[] {
    return categories
      .filter(c => (c.parentCategoryId ?? null) === parentId)
      .map(c => ({
        data: c,
        children: this.buildTreeNodes(categories, c.id),
        expanded: true
      }));
  }

  openAddRoot(): void {
    this.dialogHeader.set('Add Root Category');
    this.selectedCategoryId.set(null);
    this.selectedParentId.set(null);
    this.categoryName.set('');
    this.isDialogOpen.set(true);
  }

  openAddChild(parentNode: TreeNode): void {
    const parent = parentNode.data as Category;
    this.dialogHeader.set(`Add Subcategory under "${parent.name}"`);
    this.selectedCategoryId.set(null);
    this.selectedParentId.set(parent.id);
    this.categoryName.set('');
    this.isDialogOpen.set(true);
  }

  openEdit(node: TreeNode): void {
    const cat = node.data as Category;
    this.dialogHeader.set(`Edit Category: ${cat.name}`);
    this.selectedCategoryId.set(cat.id);
    this.selectedParentId.set(cat.parentCategoryId ?? null);
    this.categoryName.set(cat.name);
    this.isDialogOpen.set(true);
  }

  confirmDelete(node: TreeNode): void {
    const cat = node.data as Category;
    const hasChildren = node.children && node.children.length > 0;

    this.confirmationService.confirm({
      message: hasChildren
        ? `"${cat.name}" has subcategories. Are you sure you want to delete it?`
        : `Are you sure you want to delete "${cat.name}"?`,
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.deleteCategory(cat.id);
      }
    });
  }

  private deleteCategory(id: number): void {
    this.categoryService.delete(id).subscribe({
      next: () => {
        this.loadCategories();
      }
    });
  }

  saveCategory(): void {
    const currentId = this.selectedCategoryId();
    const name = this.categoryName().trim();
    const parentCategoryId = this.selectedParentId();

    if (!name) {
      return
    };

    if (currentId) {
      const updateDto: UpdateCategoryCommand = { id: currentId, name, parentCategoryId };
      this.categoryService.update(currentId, updateDto).subscribe({
        next: () => {
          this.isDialogOpen.set(false);
          this.loadCategories();
        }
      });
    } else {
      const createDto: CreateCategoryCommand = { name, parentCategoryId };
      this.categoryService.create(createDto).subscribe({
        next: () => {
          this.isDialogOpen.set(false);
          this.loadCategories();
        }
      });
    }
  }
}
