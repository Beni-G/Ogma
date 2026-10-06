import { Component, input, output, effect, signal } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { TreeModule } from 'primeng/tree';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { Category } from '../../../categories/models/category.models';

@Component({
  selector: 'app-category-filter',
  standalone: true,
  imports: [TreeModule, ButtonModule, TooltipModule],
  templateUrl: './category-filter.component.html'
})
export class CategoryFilterComponent {
  categories = input<Category[]>([]);
  activeCategoryId = input<number | null>(null);

  categoryChange = output<{
    categoryId: number | null;
    branchIds: number[];
  }>();

  categoryNodes = signal<TreeNode[]>([]);
  selectedNode = signal<TreeNode | null>(null);

  // Preserve expanded state across tree rebuilds
  private expandedKeys = new Set<string>();

  constructor() {
    effect(() => {
      const cats = this.categories();
      const activeId = this.activeCategoryId();

      if (!cats.length) {
        this.categoryNodes.set([]);
        this.selectedNode.set(null);
        return;
      }

      const nodes = this.buildTreeNodes(cats);

      if (activeId !== null) {
        const targetKey = String(activeId);
        this.expandAncestors(nodes, targetKey);

        const foundNode = this.findNode(nodes, activeId);
        this.selectedNode.set(foundNode);
      } else {
        this.selectedNode.set(null);
      }

      this.categoryNodes.set(nodes);
    });
  }

  onNodeExpand(event: { node: TreeNode }): void {
    if (event.node.key) {
      this.expandedKeys.add(event.node.key);
    }
  }

  onNodeCollapse(event: { node: TreeNode }): void {
    if (event.node.key) {
      this.expandedKeys.delete(event.node.key);
    }
  }

  selectCategory(event: { node: TreeNode }): void {
    const node = event.node;
    const categoryId = node.data?.id ?? null;

    if (categoryId === null) return;

    this.categoryChange.emit({
      categoryId,
      branchIds: this.getBranchCategoryIds(node)
    });
  }

  clearFilter(): void {
    this.selectedNode.set(null);

    this.categoryChange.emit({
      categoryId: null,
      branchIds: []
    });
  }

  private buildTreeNodes(categories: Category[]): TreeNode[] {
    if (!categories.length) return [];

    const isNested = categories.some((category: any) => 
      (category.subCategories?.length ?? 0) > 0 || (category.children?.length ?? 0) > 0
    );

    if (isNested) {
      return categories.map((category: any) => {
        const key = String(category.id);
        const children = category.subCategories ?? category.children ?? [];

        return {
          key,
          label: category.name,
          data: category,
          expanded: this.expandedKeys.has(key),
          expandedIcon: 'pi pi-folder-open',
          collapsedIcon: 'pi pi-folder',
          icon: children.length ? undefined : 'pi pi-tag',
          children: this.buildTreeNodes(children)
        };
      });
    }

    const nodeMap = new Map<number, TreeNode>();
    const rootNodes: TreeNode[] = [];

    for (const category of categories) {
      const key = String(category.id);
      nodeMap.set(category.id, {
        key,
        label: category.name,
        data: category,
        expanded: this.expandedKeys.has(key),
        expandedIcon: 'pi pi-folder-open',
        collapsedIcon: 'pi pi-folder',
        children: []
      });
    }

    for (const category of categories) {
      const parentId = (category as any).parentId ?? (category as any).parentCategoryId;
      const node = nodeMap.get(category.id)!;

      if (parentId != null && nodeMap.has(parentId)) {
        nodeMap.get(parentId)!.children!.push(node);
      } else {
        rootNodes.push(node);
      }
    }

    this.cleanUpEmptyChildren(rootNodes);
    return rootNodes;
  }

  // Traverses up to auto-expand parent nodes leading to selected child
  private expandAncestors(nodes: TreeNode[], targetKey: string): boolean {
    for (const node of nodes) {
      if (node.key === targetKey) {
        return true;
      }

      if (node.children?.length) {
        const childMatched = this.expandAncestors(node.children, targetKey);
        if (childMatched) {
          node.expanded = true;
          if (node.key) this.expandedKeys.add(node.key);
          return true;
        }
      }
    }
    return false;
  }

  private cleanUpEmptyChildren(nodes: TreeNode[]): void {
    for (const node of nodes) {
      if (node.children?.length) {
        this.cleanUpEmptyChildren(node.children);
      } else {
        node.children = undefined;
        node.icon = 'pi pi-tag';
      }
    }
  }

  private getBranchCategoryIds(node: TreeNode): number[] {
    const ids: number[] = [];
    if (node.data?.id != null) ids.push(node.data.id);
    for (const child of node.children ?? []) {
      ids.push(...this.getBranchCategoryIds(child));
    }
    return ids;
  }

  private findNode(nodes: TreeNode[], id: number | null): TreeNode | null {
    if (id === null) return null;

    for (const node of nodes) {
      if (node.data?.id === id) return node;
      const found = this.findNode(node.children ?? [], id);
      if (found) return found;
    }
    return null;
  }
}