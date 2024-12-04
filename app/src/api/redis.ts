import { Connection } from '../common/domain'
import useFetch from '../composables/useFetch'

class RedisApi {
    
    private baseUrl = 'https://localhost:7261/api/'

    async getKeysSpaces(connectionId: string, keyspace? : string) {
        const { fetchData, data, error } = await useFetch(`${this.baseUrl}redis/keyspaces?connectionId=${connectionId}&pattern=${keyspace}`)
        await fetchData()
        if(error.value)
            return null
        
        return data.value
    }

    async openConnection(payload: any) {
        const { fetchData, data, error } = await useFetch(`${this.baseUrl}redis/connections/open?connectionId=${payload.id}&host=${payload.host}&port=${payload.port}`)
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