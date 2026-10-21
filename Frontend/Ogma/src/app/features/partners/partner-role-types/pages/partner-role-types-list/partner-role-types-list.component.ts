import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { PartnerRoleTypeService } from '../../services/partner-role-type.service';
import { PartnerRoleType } from '../../models/partner-role-type.models';
import { PartnerRoleTypeFormComponent } from '../../components/partner-role-type-form/partner-role-type-form.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-partner-role-types-list',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    PartnerRoleTypeFormComponent
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './partner-role-types-list.component.html'
})
export class PartnerRoleTypesListComponent implements OnInit {
  private readonly partnerRoleTypeService = inject(PartnerRoleTypeService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  roles = signal<PartnerRoleType[]>([]);
  loading = signal<boolean>(true);

  dialogVisible = signal<boolean>(false);
  selectedRole = signal<PartnerRoleType | null>(null);

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading.set(true);
    this.partnerRoleTypeService.getAll().subscribe({
      next: (data) => {
        this.roles.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load partner role types' });
        this.loading.set(false);
      }
    });
  }

  openNew() {
    this.selectedRole.set(null);
    this.dialogVisible.set(true);
  }

  openEdit(role: PartnerRoleType) {
    this.selectedRole.set(role);
    this.dialogVisible.set(true);
  }

  deleteRole(role: PartnerRoleType) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${role.code}"?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger p-button-sm',
      rejectButtonStyleClass: 'p-button-secondary p-button-text p-button-sm',

      accept: () => {
        this.partnerRoleTypeService.delete(role.id).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Partner role type deleted successfully'
            });

            this.loadData();
          },
          error: () => {
            this.showError('Failed to delete the Partner role type.');
          }
        });
      }
    });
  }

  onSaveComplete() {
    this.dialogVisible.set(false);
    this.loadData();
  }

  private showError(detail: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail
    });
  }
}