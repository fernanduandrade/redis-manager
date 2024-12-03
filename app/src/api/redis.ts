import { Connection } from '../common/domain'
import useFetch from '../composables/useFetch'
import { keyValueRequest } from './requests'

class RedisApi {
    
    private baseUrl = 'https://localhost:7261/api/'

    async getKeyValue(payload: keyValueRequest) {
        const { fetchData, data } = await useFetch(`${this.baseUrl}redis/keyspaces/keys/aa?id=${payload.id}&host=${payload.connection.host}&port=${payload.connection.port}&cacheKey=${payload.key}`)
        await fetchData()
        
        return data.value
    }

    async getKeys(payload: keyValueRequest) {
        const { fetchData, data } = await useFetch(`${this.baseUrl}redis/keyspaces/keys/aa?id=${payload.id}&host=${payload.connection.host}&port=${payload.connection.port}&cacheKey=${payload.key}`)
        await fetchData()
        
        return data.value
    }

    async getKeysSpaces(payload: keyValueRequest) {
        const { fetchData, data } = await useFetch(`${this.baseUrl}redis/keyspaces?id=${payload.id}&host=${payload.connection.host}&port=${payload.connection.port}`)
        await fetchData()
        
        return data.value
    }

    async createConnection(payload: Connection) {
        const { fetchData, data, error } = await useFetch(`${this.baseUrl}redis/connections`, {
            method: 'POST',
            body: JSON.stringify(payload)
        })
        
        await fetchData()
        console.log(error.value)
        if(error.value)
            return null
        return data.value
    }
}

export default new RedisApi()