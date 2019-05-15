#include <eosio/eosio.hpp>
#include <string>

using namespace eosio;
using std::string;
class[[eosio::contract("onboarding")]] onboarding : public eosio::contract
{

public:
    using contract::contract;

    onboarding(name receiver, name code, datastream<const char *> ds) : contract(receiver, code, ds){};

    [[eosio::action]] void
    adduser(name account, uint64_t username) {
        user_index users(get_self(), get_self().value);

        auto itr = users.find(username);

        check(itr == users.end(), "That username already exists.");

        users.emplace(_self, [&](auto &t) {
            t.id = username;
        });
    };

    [[eosio::action]] void
    addfrag(name ork_node, string ork_url, uint64_t username, string private_key_frag, string public_key, string pass_hash, string ork_public) {
        require_auth(ork_node);

        // Get user
        user_index users(get_self(), get_self().value);
        auto itr = users.find(username);
        check(itr != users.end(), "That username does not exists.");

        // Add node to list
        users.modify(itr, _self, [&](auto &t) {
            t.nodes.push_back(node{
                ork_node,
                ork_url,
                ork_public});
        });

        // Open node scoped fragments
        frag_index ork_fragments(get_self(), ork_node.value);

        // Add fragment
        ork_fragments.emplace(_self, [&](auto &t) {
            t.id = username;
            t.private_key_frag = private_key_frag;
            t.pass_hash = pass_hash;
            t.public_key = public_key;
        });
    };

private:
    struct node
    {
        name ork_node;
        string ork_url;
        string ork_public;
    };

    struct [[eosio::table]] user
    {
        uint64_t id;
        name account;
        std::vector<node> nodes;

        uint64_t primary_key() const { return id; }
    };
    typedef eosio::multi_index<"tideusers"_n, user> user_index;

    struct [[eosio::table]] fragment
    {
        uint64_t id;
        string public_key;
        string private_key_frag;
        string pass_hash;

        uint64_t primary_key() const { return id; }
    };
    typedef eosio::multi_index<"tidefrags"_n, fragment> frag_index;
};