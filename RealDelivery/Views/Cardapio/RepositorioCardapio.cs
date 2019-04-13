using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCardapio.Models
{
    public class RepositorioCardapio
    {
        private List<Cardapio> cardapio;
        public List<Cardapio> Cardapio
        {
            get
            {
                if (cardapio == null)
                    CriarCardapio();
                return cardapio;
            }
        }

        private void CriarCardapio()
        {
            cardapio.Add(new Cardapio
            {
                grupo_cod = '0',
                grupo_nome = "",
                grupo_desc = "",
                grupo_img = ""
            });
        }
    }
}

/*
SELECT	g.grupo_cod,
        g.grupo_nome,
        g.grupo_desc,
        g.grupo_img
FROM	grupo g
WHERE	g.grupo_atv = "S"
*/
