const list = Vue.createApp({
	data() {
		return {
			isInfoModalOpen: false,
			isCartModalOpen: false,
			infoMessage: '',
			products: [],
			categories: [],
			pageNumber: 0,
			size: 9,
			searchQuery: '',
			customerId: '',
			productFilters: {
				category: '',
				minPrice: null,
				maxPrice: null,
				sortBy: 'name',
				sortDirection: 'asc',
			},
			orderItems: [],
		}
	},
	computed: {
		findMinPrice() {
			if (this.filteredProducts.length === 0) return 0;
			return Math.min(...this.filteredProducts.map(product => product.price));
		},
		findMaxPrice() {
			if (this.filteredProducts.length === 0) return 0;
			return Math.max(...this.filteredProducts.map(product => product.price));
		},
		filteredProducts() {
			let filtered = this.products;
			this.pageNumber = 0;

			if (this.searchQuery) {
				filtered = filtered.filter(product =>
					product.name.toLowerCase().includes(this.searchQuery.toLowerCase())
				);
			}

			if (this.productFilters.category) {
				filtered = filtered.filter(product => product.category.toLowerCase() === this.productFilters.category.toLowerCase());
			}

			if (this.productFilters.minPrice !== null) {
				filtered = filtered.filter(product => product.price >= this.productFilters.minPrice
				);
			}
			if (this.productFilters.maxPrice !== null) {
				filtered = filtered.filter(product => product.price <= this.productFilters.maxPrice
				);
			}

			if (this.productFilters.sortBy) {
				filtered.sort((a, b) => {
					const valueA = a[this.productFilters.sortBy];
					const valueB = b[this.productFilters.sortBy];

					if (valueA < valueB) {
						return this.productFilters.sortDirection === 'asc' ? -1 : 1;
					}
					if (valueA > valueB) {
						return this.productFilters.sortDirection === 'asc' ? 1 : -1;
					}
					return 0;
				});
			}

			return filtered;
		},
		paginatedData() {
			const start = this.pageNumber * this.size;
			const end = start + this.size;
			return this.filteredProducts.slice(start, end);
		},
		pageCount() {
			return Math.ceil(this.filteredProducts.length / this.size);
		},
		totalItemsInCart() {
			return this.orderItems.length;
		},
		getProductCount() {
			return (productId) => {
				const item = this.orderItems.find(i => i.itemId === productId);
				return item ? item.itemsCount : 0;
			};
		}
	},
	methods: {
		increaseCount(product) {
			const item = this.orderItems.find(i => i.itemId === product.id);
			if (item) {
				item.itemsCount += 1;
			} else {
				this.orderItems.push({
					id: uuid.v4(),
					orderId: '',
					itemId: product.id,
					itemsCount: 1,
					itemPrice: product.price,
					product: product,
				});
			}
		},
		decreaseCount(product) {
			const item = this.orderItems.find(i => i.itemId == product.id);
			if (item) {
				if (item.itemsCount > 1) {
					item.itemsCount -= 1;
				} else {
					this.orderItems = this.orderItems.filter(i => i.itemId !== product.id);
				}
			}
		},
		async createOrder() {
			var orderId = uuid.v4();

			var items = this.orderItems.map(({ product, ...rest }) => rest);
			items = items.map(m => {
				return {
					...m, orderId: orderId
				};
			});

			var order = {
				id: orderId,
				customerId: this.customerId,
				order_Number: 0,
				status: 'Новый',
				items: items,
			};

			var url = '/catalog/order/c';
			try {
				var response = await axios.post(url, order, {
					headers: {
						'Content-Type': 'application/json',
					}
				});
				if (response.status != 200) {
					this.openInfoModal(response.data != null || response.data != '' ? response.data : response);
					this.$forceUpdate();
					return;
				}

				this.closeCartModal();
				this.openInfoModal('Заказ успешно создан!');
				this.orderItems = [];
			}
			catch (e) {
				this.openInfoModal(e.response.data != null || e.response.data != '' ? e.response.data : e);
			}
		},
		async getProductList() {
			var url = '/catalog/products/l';
			try {
				var response = await axios.get(url);

				if (response.status != 200) {
					this.products = [];
					this.openInfoModal(response.data != null || response.data != '' ? response.data : response);
					this.$forceUpdate();
					return;
				}

				this.products = response.data.data;
				this.customerId = response.data.customerId;
				this.categories = [...new Set(this.products.map(product => product.category.toLowerCase()))];
			}
			catch (e) {
				this.products = [];
				this.openInfoModal(e.response.data != null || e.response.data != '' ? e.response.data : e);
				this.$forceUpdate();
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
		resetProductFilters() {
			this.productFilters = {
				category: '',
				minPrice: null,
				maxPrice: null,
				sortBy: 'name',
				sortDirection: 'asc',
			};
			this.searchQuery = '';
			this.pageNumber = 0;
			this.getProductList();
		},
		setSort(sortBy, sortDirection) {
			this.productFilters.sortBy = sortBy;
			this.productFilters.sortDirection = sortDirection;
			this.pageNumber = 0;
		},
		openInfoModal(message) {
			this.isInfoModalOpen = true;
			this.infoMessage = message;
		},
		closeInfoModal() {
			this.isInfoModalOpen = false;
		},
		openCartModal() {
			this.isCartModalOpen = true;
		},
		closeCartModal() {
			this.isCartModalOpen = false;
		},
		deleteCartItem(id) {
			this.orderItems = this.orderItems.filter(i => i.id !== id)
			if (this.orderItems.length === 0) {
				this.closeCartModal();
			}
		},
	},
	beforeMount() {
		this.getProductList();
	}
})
list.mount('#ordersCreate');