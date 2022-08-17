﻿using PeixeLegal.Src.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using PeixeLegal.Src.Modelos;
using Microsoft.AspNetCore.Authorization;

namespace PeixeLegal.Src.Controladores
{
    [ApiController]
    [Route("api/Produtos")]
    [Produces("application/json")]
    public class ProdutosControlador : ControllerBase
    {
        #region Atributos
        private readonly IProdutos _repositorio;
        #endregion
        #region Construtores
        public ProdutosControlador(IProdutos repositorio)
        {
            _repositorio = repositorio;
        }
        #endregion
        #region Métodos
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> PegarTodosProdutosAsync()
        {
            var lista = await _repositorio.PegarTodosProdutosAsync();
            if (lista.Count < 1) return NoContent();
            return Ok(lista);
        }
        [HttpGet("id/{idProdutos}")]
        [Authorize]
        public async Task<ActionResult> PegarProdutosPeloIdAsync([FromRoute] int id_Produto)
        {
            try
            {
                return Ok(await _repositorio.PegarProdutosPeloIdAsync(id_Produto));
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> NovoProdutosAsync([FromBody] Produtos produtos)
        {
            await _repositorio.NovoProdutosAsync(produtos);
            return Created($"api/Produtos", produtos);
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> AtualizarProdutosAsync([FromBody] Produtos produto)
        {
            try
            {
                await _repositorio.AtualizarProdutosAsync(produto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }
        [HttpDelete("deletar/{idProdutos}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> DeletarProdutosAsync([FromRoute] int id_Produto)
        {
            try
            {
                await _repositorio.DeletarProdutosAsync(id_Produto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        #endregion
    }
}
