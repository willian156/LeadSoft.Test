import { useEffect, useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";

export default function StartProcess() {
    const [type, setType]= useState("");
    const [source, setSource] = useState("")
    const [user, setUser] = useState()
    const [users, setUsers] = useState([])
    const API_URL = import.meta.env.VITE_APP_API_URL;

    useEffect(() => {
        if (Cookies.get('role') != 0) {
            window.location.href = '/';
        }
    }, [])

    useEffect(() => {
        GetUsers()
    },[])

    const GetUsers = () => {

        let AdminId = Cookies.get('id');

        let config = {
            method: 'get',
            maxBodyLength: Infinity,
            url: API_URL + '/GetUsers/' + AdminId,
            headers: {}
        };

        axios.request(config)
            .then((response) => {
                let test = response.data;
                setUsers(test);
            })
            .catch((error) => {
                console.log(error);
            });

    }

    const StartProcessing = async (e) => {
        e.preventDefault();
          
        let data = JSON.stringify({
            "userId": user.id,
            "type": type,
            "source": source
        });


        let config = {
            method: 'post',
            maxBodyLength: Infinity,
            url: API_URL + '/Animes/StartProcessing/' + Cookies.get("id"),
            headers: {
                'Content-Type': 'application/json'
            },
            data: data
        };

        axios.request(config)
            .then((response) => {
                setMsg(response.data.message);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    const CancelProcess = async (e) => {
        e.preventDefault()
          
        let config = {
            method: 'post',
            maxBodyLength: Infinity,
            url: API_URL+'/Animes/CancelProcessing?groupId='+user.id+"_"+user.username,
            headers: { }
          };
          
          axios.request(config)
          .then((response) => {
            console.log(JSON.stringify(response.data));
          })
          .catch((error) => {
            console.log(error);
          });
    }


    return (
        <div>
            <div className="space-y-4">
                <button
                    onClick={() => { window.location.href = "/" }}
                    className="w-40 bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition"
                >
                    voltar à tela principal
                </button>
                <h2>Criar Usuário</h2>
                <form onSubmit={StartProcessing} className="space-y-4">
                <div>
                        <label className="block text-gray-700">Usuários</label>
                        <select
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={user?.id || ""}
                            onChange={(e) => {
                                const selectedUser = users.find((u) => u.id === parseInt(e.target.value));
                                setUser(selectedUser);
                            }}

                        >
                            <option disabled value="">
                                Selecione um Usuário
                            </option>
                            {users.map((item) => (
                                <option key={item.id} value={item.id}>
                                    |{item.id}| {item.username} - {item.role === 0 ? "Admin" : "Basic"}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label className="block text-gray-700">Tipo</label>
                        <select
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={type}
                            onChange={(e) => setType(e.target.value)}

                        >
                            <option value="TV">TV</option>
                            <option value="OVA">OVA</option>
                            <option value="Movie">Movie</option>
                            <option value="Special">Special</option>
                            <option value="ONA">ONA</option>  
                        </select>
                    </div>
                    <div>
                        <label className="block text-gray-700">Fonte</label>
                        <select
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={source}
                            onChange={(e) => setSource(e.target.value)}

                        >
                            <option value="Manga">Manga</option>
                            <option value="Original">Original</option>
                            <option value="Light novel">Light novel</option>
                            <option value="4-koma manga">4-koma manga</option>
                            <option value="Unknown">Unknown</option>
                            <option value="Visual novel">Visual novel</option>
                            <option value="Novel">Novel</option>
                            <option value="Game">Game</option>
                            <option value="Other">Other</option>
                            
                        </select>
                    </div>
                    <button
                        type="submit"
                        className="w-full bg-green-500 text-white py-3 rounded-lg hover:bg-green-600 transition"
                    >
                        Iniciar Processamento
                    </button>
                </form>
                <button
                    onClick={(e) => CancelProcess(e)}
                    className="w-full bg-red-500 text-white py-3 rounded-lg hover:bg-red-600 transition"
                >
                    Cancelar Processamento
                </button>

            </div>
        </div>
    );
}