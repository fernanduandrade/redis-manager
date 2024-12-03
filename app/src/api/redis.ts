import { Connection } from '../common/domain'
import useFetch from '../composables/useFetch'
import { keyValueRequest } from './requests'

class RedisApi {
    
    private baseUrl = 'https://localhost:7261/api/'

    async getKeysSpaces(payload: any, keyspace? : string) {
        const { fetchData, data, error } = await useFetch(`${this.baseUrl}redis/keyspaces?id=${payload.id}&host=${payload.host}&port=${payload.port}&keyspace=${keyspace}`)
        await fetchData()
        if(error.value)
            return null
        
        return data.value
    }

    async createConnection(payload: Connection) {
        const { fetchData, data, error } = await useFetch(`${this.baseUrl}redis/connections`, {
            method: 'POST',
            body: JSON.stringify(payload)
        })
        
        await fetchData()
        if(error.value)
            return null
        return data.value
    }
}

export default new RedisApi()