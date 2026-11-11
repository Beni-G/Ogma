import { Component, OnInit, inject, signal, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Partner, PartnerContact } from '../../models/partner.models';
import { PartnerService } from '../../services/partner.service';
import { PartnerFormDialogComponent } from '../../components/partner-form-dialog/partner-form-dialog.component';

export interface PartnerTableItem extends Partner {
  fullName: string;
  typeDisplay: string;
  primaryIdentifierDisplay: string;
  rolesDisplay: string;
}

interface Column {
  field: string;
  header: string;
}

@Component({
  selector: 'app-partners-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    MultiSelectModule,
    SelectModule,
    PartnerFormDialogComponent
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './partners-list.component.html'
})
export class PartnersListComponent implements OnInit {
  private partnerService = inject(PartnerService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  partners = signal<PartnerTableItem[]>([]);
  loading = signal<boolean>(true);
  dialogVisible = signal<boolean>(false);
  selectedPartner = signal<Partner | null>(null);

  expandedRows = signal<{ [key: string]: boolean }>({});

  statusOptions = [
    { label: 'Active', value: true },
    { label: 'Inactive', value: false }
  ];

  cols: Column[] = [
    { field: 'fullName', header: 'Name' },
    { field: 'displayName', header: 'Display Name' },
    { field: 'typeDisplay', header: 'Type' },
    { field: 'primaryIdentifierDisplay', header: 'Primary Identifier' },
    { field: 'rolesDisplay', header: 'Role Types' },
    { field: 'isActive', header: 'Status' }
  ];

  selectedColumns = signal<Column[]>([...this.cols]);

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading.set(true);
    this.partnerService.getAll().subscribe({
      next: (data) => {
        const mapped: PartnerTableItem[] = data.map(p => {
          p.contacts?.sort((a, b) => {

            const primaryA = a.isPrimary ? 1 : 0;
            const primaryB = b.isPrimary ? 1 : 0;
            if (primaryA !== primaryB) {
              return primaryB - primaryA; 
            }

            const nameA = `${a.name?.firstName || ''} ${a.name?.lastName || ''}`.trim();
            const nameB = `${b.name?.firstName || ''} ${b.name?.lastName || ''}`.trim();
            return nameA.localeCompare(nameB);
          });
          const primaryId = p.identifiers?.find(i => i.isPrimary) || p.identifiers?.[0];
          return {
            ...p,
            fullName: p.isNaturalPerson
              ? `${p.individualName?.firstName || ''} ${p.individualName?.lastName || ''}`.trim()
              : (p.companyName || ''),
            typeDisplay: p.isNaturalPerson ? 'Natural Person' : 'Company',
            primaryIdentifierDisplay: primaryId ? `${primaryId.type}: ${primaryId.value}` : '-',
            rolesDisplay: p.roles ? p.roles.map(r => r.name).join(', ') : '-'
          };
        });

        this.partners.set(mapped);
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load partners' });
        this.loading.set(false);
      }
    });
  }

  isRowExpanded(partner: PartnerTableItem): boolean {
    return !!this.expandedRows()[String(partner.id)];
  }

  toggleRow(partner: PartnerTableItem) {
    const id = String(partner.id);
    this.expandedRows.update(current => {
      const next = { ...current };
      if (next[id]) {
        delete next[id];
      } else {
        next[id] = true;
      }
      return next;
    });
  }

  openNew() {
    this.selectedPartner.set(null);
    this.dialogVisible.set(true);
  }

  openEdit(partner: Partner) {
    this.selectedPartner.set(partner);
    this.dialogVisible.set(true);
  }

  deletePartner(partner: Partner) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete partner?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger p-button-sm',
      rejectButtonStyleClass: 'p-button-secondary p-button-text p-button-sm',
      accept: () => {
        this.partnerService.delete(partner.id).subscribe({
          next: () => {
            this.partners.update(list => list.filter(p => p.id !== partner.id));
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Partner deleted successfully' });
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete partner' });
          }
        });
      }
    });
  }

  onSaveComplete() {
    this.dialogVisible.set(false);
    this.loadData();
  }

  getSecondaryIdentifiers(partner: Partner) {
    return partner.identifiers?.filter(i => !i.isPrimary) || [];
  }

  formatDate(date?: string | null): string {
    if (!date) return '';
    return date.split('T')[0];
  }

  formatValidityPeriod(period?: { start?: string | null; end?: string | null }): string {
    if (!period?.start) return '';

    const start = this.formatDate(period.start);
    const end = this.formatDate(period.end);

    if (end) {
      return `(${start} – ${end})`;
    }

    return `(${start} – ongoing)`;
  }

  formatContact(contact: PartnerContact | null): string {
    if (!contact) return '';
    let formattedContact = '';
    formattedContact += contact.title ? `<strong>${contact.title}</strong> ` : '';
    formattedContact += `${contact.name.firstName} ${contact.name.lastName}`;
    formattedContact += contact.jobTitle ? ` - ${contact.jobTitle}` : '';
    formattedContact += contact.email ? `<br><strong>Email:</strong> ${contact.email}` : '';
    formattedContact += contact.mobile ? `<br><strong>Mobile:</strong> ${contact.mobile}` : '';
    formattedContact += contact.phone ? `<br><strong>Phone:</strong> ${contact.phone}` : '';
    formattedContact += contact.isPrimary ? '<br><span class="font-medium text-xs bg-blue-100 text-blue-700 px-1.5 py-0.5 rounded">Primary contact</span>' : '';
    return formattedContact;
  }

  formatAddress(addr: any): string {
    if (!addr) return 'No HQ Address provided';
    const parts = [addr.street, addr.number, addr.city, addr.postalCode, addr.region, addr.countryCode].filter(Boolean);
    return parts.join(', ');
  }
}