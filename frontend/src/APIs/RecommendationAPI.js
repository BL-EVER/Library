import axios from 'axios';
import { toast } from 'react-toastify';

export default class RecommendationAPI {
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

    getByPopularity = async (amount) => {
        try {
            const response = await this.api.get('/api/order/Recommendation/ByPopularity', { params: { amount } });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    getByHistory = async (amountPerBlock) => {
        try {
            const response = await this.api.get('/api/order/Recommendation/ByHistory', { params: { amountPerBlock } });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }
}