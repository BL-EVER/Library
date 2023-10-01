import axios from 'axios';
import { toast } from 'react-toastify';

export default class OrderAPI {
    constructor(accessToken) {
        this.api = axios.create({
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${accessToken}`,
            },
        });
    }

    async handleResponse(response, successMessage) {
        if (response.status >= 200 && response.status <= 299) {
            toast.success(successMessage);
            return response.data;
        } else {
            toast.error(`Error: ${response.status}`);
            throw new Error(`Error: ${response.status}`);
        }
    }

    getStock = async (ISBNs) => {
        try {
            const response = await this.api.get('/api/order/Order/stock', { params: { ISBNs } });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    getOrders = async (params) => {
        try {
            const response = await this.api.get('/api/order/Order', { params });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    getOrder = async (id) => {
        try {
            const response = await this.api.get(`/api/order/Order/${id}`);
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    createOrder = async (data) => {
        try {
            const response = await this.api.post('/api/order/Order', data);
            return this.handleResponse(response, 'Order created successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    patchOrder = async (id, data) => {
        try {
            const response = await this.api.patch(`/api/order/Order/${id}`, data);
            return this.handleResponse(response, 'Order updated successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    deleteOrder = async (id) => {
        try {
            const response = await this.api.delete(`/api/order/Order/${id}`);
            return this.handleResponse(response, 'Order deleted successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }
}