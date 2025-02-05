const list = Vue.createApp({
	data() {
		return {
			isInfoModalOpen: false,
			isDeleteModalOpen: false,
			message: '',
			deleteId: '',
			orders: [],
			statuses: ['Новый', 'Выполняется', 'Выполнен'],
			selectedStatus: '',
			pageNumber: 0,
			size: 9,
		}
	},
	computed: {
		filteredOrders() {
			let filtered = this.orders;
			this.pageNumber = 0;

			if (this.selectedStatus) {
				filtered = filtered.filter(order => order.status.toLowerCase() === this.selectedStatus.toLowerCase());
			}

			return filtered;
		},
		paginatedData() {
			const start = this.pageNumber * this.size;
			const end = start + this.size;
			return this.filteredOrders.slice(start, end);
		},
		pageCount() {
			return Math.ceil(this.filteredOrders.length / this.size);
		},
		getOrdersCount() {
			return (productId) => {
				const item = this.orderItems.find(i => i.itemId === productId);
				return item ? item.itemsCount : 0;
			};
		}
	},
	methods: {
		async getOrdersList() {
			var url = '/orders/l';
			try {
				var response = await axios.get(url);

				if (response.status != 200) {
					this.orders = [];
					this.openInfoModal(response.data != null || response.data != '' ? response.data : response);
					return;
				}

				this.orders = response.data;
			}
			catch (e) {
				this.orders = [];
				this.openInfoModal(e.response.data != null || e.response.data != '' ? e.response.data : e);
			}
		},
		async deleteOrder() {
			var url = `/orders/d/${this.deleteId}`;
			try {
				var response = await axios.post(url);
				this.closeDeleteModal();

				if (response.status != 200) {
					this.openInfoModal(response.data != null || response.data != '' ? response.data : response);
					return;
				}

				this.openInfoModal(response.data);
				this.getOrdersList();
			}
			catch (e) {
				this.closeDeleteModal();
				this.openInfoModal(e.response.data != null || e.response.data != '' ? e.response.data : e);
            }
		},
		nextPage() {
			if (this.pageNumber < this.pageCount - 1) {
				this.pageNumber++;
			}
		},
		previousPage() {
			if (this.pageNumber > 0) {
				this.pageNumber--;
			}
		},
		resetOrderFilters() {
			this.selectedStatus = '';
			this.pageNumber = 0;
			this.getOrdersList();
		},
		openDeleteModal(id) {
			this.deleteId = id;
			this.message = `Вы действительно хотите удалить заказ ${id}`;
			this.isDeleteModalOpen = true;
		},
		closeDeleteModal() {
			this.isDeleteModalOpen = false;
		},
		openInfoModal(message) {
			this.message = message;
			this.isInfoModalOpen = true;
		},
		closeInfoModal() {
			this.isInfoModalOpen = false;
		}
	},
	beforeMount() {
		this.getOrdersList();
	}
})
list.mount('#orders');