using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace XF.Recursos.Lista
{
    public class ViewModelProdutos : INotifyPropertyChanged
    {
        private string pesquisaPorDescricao;
        public string PesquisaPorDescricao
        {
            get { return pesquisaPorDescricao; }
            set
            {
                if (value == pesquisaPorDescricao) return;

                pesquisaPorDescricao = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorDescricao)));
                AplicarFiltro();
            }
        }

        public List<Produto> ProdutosFiltrado { get; set; } = new List<Produto>();
        public ObservableCollection<Produto> Produtos { get; set; } = new ObservableCollection<Produto>();

        public ViewModelProdutos() { }

        public void AplicarFiltro()
        {
            if (PesquisaPorDescricao == null) PesquisaPorDescricao = "";

            var resultado = ProdutosFiltrado.Where(n => n.Descricao.ToLowerInvariant()
                                .Contains(pesquisaPorDescricao.ToLowerInvariant().Trim())).ToList();

            var removerDaLista = Produtos.Except(resultado).ToList();
            foreach (var item in removerDaLista)
            {
                Produtos.Remove(item);
            }

            for (int index = 0; index < resultado.Count; index++)
            {
                var item = resultado[index];
                if (index + 1 > Produtos.Count || !Produtos[index].Equals(item))
                    Produtos.Insert(index, item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
