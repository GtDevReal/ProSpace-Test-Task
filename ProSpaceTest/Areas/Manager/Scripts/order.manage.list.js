const list = Vue.createApp({
	data() {
		return {
			isInfoModalOpen: false,
			isOrderEditModalOpen: false,
			isCompleteModalOpen: false,
			order: '',
			message: '',
			orders: [],
			statuses: ['Новый', 'Выполняется', 'Выполнен'],
			selectedStatus: '',
			pageNumber: 0,
			size: 9,
		}
	},
	computed: {
		tomorrowDate() {
			const today = new Date();
			const tomorrow = new Date(today);
			tomorrow.setDate(today.getDate() + 1);

			return tomorrow.toISOString().split('T')[0];
		},
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
			var url = '/manage/l';
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
		async editOrder(num) {
			var url = '/manage/e';

			if (num == 1) {
				this.order.status = 'Выполнен';
			}

			try {
				var token = $("input[name = '__RequestVerificationToken']").val()
				var response = await axios.post(url, this.order, {
					headers: {
						'Content-Type': 'application/json',
						'RequestVerificationToken': token
					}
				});

				this.closeOrderEditModal();
				this.closeCompleteModal();

				if (response.status != 200) {
					this.openInfoModal(response.data != null || response.data != '' ? response.data : response);
					return;
				}

				this.openInfoModal(response.data);
				this.getOrdersList();
			}
			catch (e) {
				this.closeOrderEditModal();
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
		openInfoModal(message) {
			this.message = message;
			this.isInfoModalOpen = true;
		},
		closeInfoModal() {
			this.isInfoModalOpen = false;
		},
		openOrderEditModal(order) {
			this.order = order;
			this.isOrderEditModalOpen = true;
		},
		closeOrderEditModal() {
			this.order = '';
			this.isOrderEditModalOpen = false;
			this.getOrdersList();
		},
		openCompleteModal(id) {
			this.message = `Подтвердить выполнение заказа ${id}`;
			this.isCompleteModalOpen = true;
		},
		closeCompleteModal() {
			this.order = '';
			this.isCompleteModalOpen = false;
			this.getOrdersList();
		},
	},
	beforeMount() {
		this.getOrdersList();
	}
})
list.mount('#orders');