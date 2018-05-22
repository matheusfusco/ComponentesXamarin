using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.Recursos.Lista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProdutoView : ContentPage
	{
        ViewModelProdutos _vmProdutos;

        public ProdutoView()
        {
            if (_vmProdutos == null)
                _vmProdutos = new ViewModelProdutos();

            BindingContext = _vmProdutos;

            InitializeComponent();
            LoadProdutos();
        }

        private async void LoadProdutos()
        {
            var httpRequest = new HttpClient();
            var stream = await httpRequest.GetStringAsync("https://apiaplicativofiap.azurewebsites.net/content/xml/produtos.xml");

            XElement xmlProduto = XElement.Parse(stream);
            foreach (var item in xmlProduto.Elements("produto"))
            {
                Produto produto = new Produto()
                {
                    Id = int.Parse(item.Attribute("id").Value),
                    Descricao = item.Element("descricao").Value,
                    Categoria = item.Element("categoria").Value,
                    Quantidade = int.Parse(item.Element("quantidade").Value),
                    Preco = decimal.Parse(item.Element("precounitario").Value)
                };
                _vmProdutos.ProdutosFiltrado.Add(produto);
                _vmProdutos.AplicarFiltro();
            }
        }

        private void lstProduto_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemSelecionado = e.Item as Produto;
            DisplayAlert("Produto selecionado",
                $"Id: {itemSelecionado.Id} - {itemSelecionado.Descricao}", "OK");
        }
    }
}